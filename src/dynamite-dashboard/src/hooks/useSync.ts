import { useEffect, useRef } from 'react'
import * as signalR from '@microsoft/signalr'
import { useQueryClient } from '@tanstack/react-query'
import { useParams } from 'react-router-dom'
import { toast } from 'react-hot-toast'

export function useSync() {
    const { guildId } = useParams<{ guildId: string }>()
    const queryClient = useQueryClient()
    const connectionRef = useRef<signalR.HubConnection | null>(null)

    useEffect(() => {
        if (!guildId) return

        const connection = new signalR.HubConnectionBuilder()
            .withUrl(`${import.meta.env.VITE_API_URL}/hubs/sync`)
            .withAutomaticReconnect()
            .build()

        connectionRef.current = connection

        connection.on('ConfigUpdated', (updatedGuildId: string) => {
            // SignalR might send guildId as string or number depending on Npgsql mapping
            if (updatedGuildId.toString() === guildId) {
                // Invalidate query cache for this guild to instantly fetch new data
                queryClient.invalidateQueries({ queryKey: ['guild', guildId] })
            }
        })

        connection.on('ModuleFaulted', (faultedGuildId: string, moduleName: string, reason: string) => {
            if (faultedGuildId.toString() === guildId) {
                queryClient.invalidateQueries({ queryKey: ['guild', guildId] })
                toast.error(`Tính năng ${moduleName} vừa bị vô hiệu hóa tự động do lỗi liên tục.\nLý do: ${reason}`, {
                    duration: 8000,
                    position: 'top-right',
                })
            }
        })

        connection.start()
            .then(() => console.log('SignalR connected for real-time sync'))
            .catch(err => console.error('SignalR connection error:', err))

        return () => {
            connection.stop()
        }
    }, [guildId, queryClient])
}
